using ChemicalApp.Data;
using ChemicalApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemicalApp
{
    /// <summary>
    /// Интерфейс работы с БД
    /// </summary>
    public interface IDataAccessService
    {
        ObservableCollection<Product> GetProducts();
        ObservableCollection<MainData> GetMainDatas(DateTime start, DateTime end);
        ObservableCollection<Balance> GetBalances();
        ObservableCollection<Debet> GetDebets();
        ObservableCollection<Kredit> GetKredits();
        ObservableCollection<Department> GetDepartments();
        ObservableCollection<DepartmentKredit> GetDepartmentKredits(int id, DateTime start, DateTime end);

        Task<int> CreateProduct(Product product);
        Task<int> CreateBalance(Balance balance);
        Task<int> CreateDebet(Debet debet);
        Task<int> CreateKredit(Kredit kredit);

        int Balance(int id, DateTime end);
    }

    public class DataAccessService : IDataAccessService
    {
        /// <summary>
        /// Получение данных для интерфейса
        /// </summary>
        /// <returns>Коллекция данных</returns>
        public ObservableCollection<MainData> GetMainDatas(DateTime start, DateTime end)
        {
            ObservableCollection<MainData> mains = new ObservableCollection<MainData>();
            foreach (var item in GetProducts())
            {
                mains.Add(new MainData()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Summa = Balance(item.Id, end),
                    Debet = GetDebet(item.Id, start, end),
                    DepartmentKredits = GetDepartmentKredits(item.Id, start, end)
                });
            }
            return mains;
        }
        /// <summary>
        /// Получение данных расхода
        /// </summary>
        /// <param name="id">ID продукции</param>
        /// <returns>Массив данных</returns>
        public ObservableCollection<DepartmentKredit> GetDepartmentKredits(int id, DateTime start, DateTime end)
        {            
            ObservableCollection<DepartmentKredit> model = new ObservableCollection<DepartmentKredit>();
            foreach (var item in GetDepartments())
            {
                model.Add(new DepartmentKredit()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Sum = (from k in GetKredits() where k.ProductId == id && k.DepartmentId == item.Id && k.Date >= start && k.Date <=end select k.Sum).Sum()
                });
            }
            return model;
        }
        /// <summary>
        /// Получение данных из таблицы Product
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Product> GetProducts()
        {
            using (var db = new ChemicalContext())
                return new ObservableCollection<Product>(db.Products.ToArray());
        }
        /// <summary>
        /// Получение данных из таблицы Debet
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Debet> GetDebets()
        {
            using (var db = new ChemicalContext())
                return new ObservableCollection<Debet>(db.Debets.ToArray());
        }
        /// <summary>
        /// Получение данных из таблицы Kredit
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Kredit> GetKredits()
        {
            using (var db = new ChemicalContext())
                return new ObservableCollection<Kredit>(db.Kredits.ToArray());
        }
        /// <summary>
        /// Получение данных из таблицы Balance
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Balance> GetBalances()
        {
            using (var db = new ChemicalContext())
                return new ObservableCollection<Balance>(db.Balances.ToArray());
        }
        /// <summary>
        /// Получение данных из таблицы Debet
        /// </summary>
        /// <param name="id">ID Product</param>
        /// <returns></returns>
        public int GetDebet(int id, DateTime start, DateTime end)
        {            
            return (from d in GetDebets() where d.Date>=start && d.Date<=end && d.ProductId == id select d.Sum).Sum();
        }
        /// <summary>
        /// Получение остатка
        /// </summary>
        /// <param name="id">ID Product</param>
        /// <returns></returns>
        public int Balance(int id, DateTime end)
        {
            int begin = (from b in GetBalances() where b.ProductId == id select b.Sum).FirstOrDefault();
            int debet = (from d in GetDebets() where d.Date <= end && d.ProductId == id select d.Sum).Sum();
            int kredit = (from k in GetKredits() where k.Date <= end && k.ProductId == id select k.Sum).Sum();
            return begin + debet - kredit;
        }
        /// <summary>
        /// Добавлеие записи в таблицу Product
        /// </summary>
        /// <param name="product">Данные для добавления</param>
        /// <returns></returns>
        public async Task<int> CreateProduct(Product product)
        {
            using (var db = new ChemicalContext())
            {
                db.Products.AddOrUpdate(product);
                if (await db.SaveChangesAsync().ConfigureAwait(false) > 0)
                    return product.Id;
            }
            return 0;
        }
        /// <summary>
        /// Добавление записи в таблицу Balance
        /// </summary>
        /// <param name="balance">Данные для добавления</param>
        /// <returns></returns>
        public async Task<int> CreateBalance(Balance balance)
        {
            using (var db = new ChemicalContext())
            {
                db.Balances.AddOrUpdate(balance);
                if (await db.SaveChangesAsync().ConfigureAwait(false) > 0)
                    return balance.Id;
            }
            return 0;
        }
        /// <summary>
        /// Получение данных из таблицы Department
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Department> GetDepartments()
        {
            using (var db = new ChemicalContext())
                return new ObservableCollection<Department>(db.Departments.ToArray());
        }
        /// <summary>
        /// Добавление записи в таблицу Debet
        /// </summary>
        /// <param name="debet">Данные для добавления</param>
        /// <returns></returns>
        public async Task<int> CreateDebet(Debet debet)
        {
            using (var db=new ChemicalContext())
            {
                db.Debets.AddOrUpdate(debet);
                if (await db.SaveChangesAsync().ConfigureAwait(false) > 0)
                    return debet.Id;
            }
            return 0;
        }
        /// <summary>
        /// Добавление записи в таблицу Kredit
        /// </summary>
        /// <param name="kredit">Данные для добавления</param>
        /// <returns></returns>
        public async Task<int> CreateKredit(Kredit kredit)
        {
            using (var db=new ChemicalContext())
            {
                db.Kredits.AddOrUpdate(kredit);
                if (await db.SaveChangesAsync().ConfigureAwait(false) > 0)
                    return kredit.Id;
            }
            return 0;
        }
    }
}
