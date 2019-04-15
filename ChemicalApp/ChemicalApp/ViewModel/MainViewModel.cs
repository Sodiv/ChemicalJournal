using ChemicalApp.Data;
using ChemicalApp.Model;
using ChemicalApp.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChemicalApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataAccessService _DataAccessService;

        private string _Title = "Химия";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private ObservableCollection<Product> _Products;
        public ObservableCollection<Product> Products
        {
            get => _Products;
            private set => Set(ref _Products, value);
        }

        private ObservableCollection<MainData> _MainDatas;
        public ObservableCollection<MainData> MainDatas
        {
            get => _MainDatas;
            private set => Set(ref _MainDatas, value);
        }

        public ICommand UpdateDataCommand { get; }
        public ICommand CreateNewProduct { get; }
        public ICommand SaveProduct { get; }
        public ICommand SaveDebet { get; }
        public ICommand Cancel { get; }
        public ICommand AddDebet { get; }
        public ICommand AddKredit { get; }
        public ICommand SaveKredit { get; }
        public ICommand DateNow { get; }

        AddProduct addProduct;
        AddDebet addDebet;
        AddKredit addKredit;
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;

        private MainData _SelectedMainData;
        public MainData SelectedMainData
        {
            get => _SelectedMainData;
            set => Set(ref _SelectedMainData, value);
        }

        private Product _CurrentProduct;
        public Product CurrentProduct
        {
            get => _CurrentProduct;
            set => Set(ref _CurrentProduct, value);
        }

        private Balance _CurrentBalance;
        public Balance CurrentBalance
        {
            get => _CurrentBalance;
            set => Set(ref _CurrentBalance, value);
        }

        private Debet _CurrentDebet;
        public Debet CurrentDebet
        {
            get => _CurrentDebet;
            set => Set(ref _CurrentDebet, value);
        }

        private Kredit _CurrentKredit;
        public Kredit CurrentKredit
        {
            get => _CurrentKredit;
            set => Set(ref _CurrentKredit, value);
        }

        private DateTime _SelectStartDate;
        public DateTime SelectStartDate
        {
            get => _SelectStartDate;
            set => Set(ref _SelectStartDate, value);
        }

        private DateTime _SelectEndDate;
        public DateTime SelectEndDate
        {
            get => _SelectEndDate;
            set => Set(ref _SelectEndDate, value);
        }

        public MainViewModel(IDataAccessService DataAccessService)
        {
            _DataAccessService = DataAccessService;

            SelectStartDate = new DateTime(year, month, 1);
            SelectEndDate = SelectStartDate.AddMonths(1).AddDays(-1);

            UpdateDataCommand = new RelayCommand(OnUpdateDataCommandExecuted, UpdateDataCommandCanExecuted);
            CreateNewProduct = new RelayCommand(OnCreateNewProductExecuted);
            SaveProduct = new RelayCommand(OnSaveProductExecuted);
            SaveDebet = new RelayCommand(OnSaveDebetExecuted);
            SaveKredit = new RelayCommand(OnSaveKreditExecuted);
            Cancel = new RelayCommand(OnCancelExecuted);
            AddDebet = new RelayCommand<int>(OnAddDebetExecuted);
            AddKredit = new RelayCommand<int>(OnAddKreditExecuted);
            DateNow = new RelayCommand(OnDateNowExecuted);
            MainDatas = _DataAccessService.GetMainDatas(SelectStartDate, SelectEndDate);
        }

        private void OnDateNowExecuted()
        {
            SelectStartDate = new DateTime(year, month, 1);
            SelectEndDate = SelectStartDate.AddMonths(1).AddDays(-1);
            OnUpdateDataCommandExecuted();
        }

        private async void OnSaveKreditExecuted()
        {
            if (await _DataAccessService.CreateKredit(CurrentKredit) > 0)
                OnUpdateDataCommandExecuted();
            addKredit.Close();
        }

        private void OnAddKreditExecuted(int id)
        {
            CurrentKredit = new Kredit();
            CurrentKredit.ProductId = SelectedMainData.Id;
            CurrentKredit.DepartmentId = id;
            CurrentKredit.Date = DateTime.Now;
            addKredit = new AddKredit();
            addKredit.Title = $"Расход {SelectedMainData.Name}";
            addKredit.ShowDialog();
        }

        private void OnUpdateDataCommandExecuted()
        {
            MainDatas = _DataAccessService.GetMainDatas(SelectStartDate, SelectEndDate);
            RaisePropertyChanged(nameof(MainDatas));
        }

        private bool UpdateDataCommandCanExecuted() => true;

        private async void OnSaveDebetExecuted()
        {
            if (await _DataAccessService.CreateDebet(CurrentDebet) > 0)
                if (CurrentDebet.Date.Month == DateTime.Today.Month)
                    SelectedMainData.Debet += CurrentDebet.Sum;
            addDebet.Close();
        }

        private void OnAddDebetExecuted(int id)
        {
            CurrentDebet = new Debet();
            CurrentDebet.ProductId = id;
            CurrentDebet.Date = DateTime.Now;
            addDebet = new AddDebet();
            addDebet.Title = $"Приход {SelectedMainData.Name}";
            addDebet.ShowDialog();
        }

        private void OnCancelExecuted()
        {
            this.addProduct.Close();
        }

        private async void OnSaveProductExecuted()
        {
            if (await _DataAccessService.CreateProduct(CurrentProduct) > 0)
            {
                CurrentBalance.ProductId = CurrentProduct.Id;
                if (await _DataAccessService.CreateBalance(CurrentBalance) > 0)
                {
                    MainDatas.Add(new MainData()
                    {
                        Id = CurrentProduct.Id,
                        Name = CurrentProduct.Name,
                        Summa = _DataAccessService.Balance(CurrentProduct.Id, SelectEndDate),
                        DepartmentKredits = _DataAccessService.GetDepartmentKredits(CurrentProduct.Id, SelectStartDate, SelectEndDate)
                    });
                    this.addProduct.Close();
                }
            }
        }

        private void OnCreateNewProductExecuted()
        {
            CurrentProduct = new Product();
            CurrentBalance = new Balance();
            addProduct = new AddProduct();
            addProduct.Title = "Добавить товар";
            addProduct.ShowDialog();
        }
    }
}