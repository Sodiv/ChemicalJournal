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
    public interface IDataAccessService
    {
        ObservableCollection<Product> GetProducts();
        ObservableCollection<MainData> GetMainDatas();
        ObservableCollection<Balance> GetBalances();
        ObservableCollection<Debet> GetDebets();
        ObservableCollection<Kredit> GetKredits();

        Task<int> CreateProduct(Product product);
        Task<int> CreateBalance(Balance balance);
    }

    public class DataAccessService : IDataAccessService
    {
        public ObservableCollection<MainData> GetMainDatas()
        {
            ObservableCollection<MainData> mains = new ObservableCollection<MainData>();
            foreach (var item in GetProducts())
            {
                mains.Add(new MainData()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Summa = Balance(item.Id)
                });
            }
            return mains;
        }

        public ObservableCollection<Product> GetProducts()
        {
            using (var db = new ChemicalContext())
                return new ObservableCollection<Product>(db.Products.ToArray());
        }

        public ObservableCollection<Debet> GetDebets()
        {
            using (var db = new ChemicalContext())
                return new ObservableCollection<Debet>(db.Debets.ToArray());
        }

        public ObservableCollection<Kredit> GetKredits()
        {
            using (var db = new ChemicalContext())
                return new ObservableCollection<Kredit>(db.Kredits.ToArray());
        }

        public ObservableCollection<Balance> GetBalances()
        {
            using (var db = new ChemicalContext())
                return new ObservableCollection<Balance>(db.Balances.ToArray());
        }

        public int Balance(int id)
        {
            int begin = (from b in GetBalances() where b.ProductId == id select b.Sum).FirstOrDefault();
            int debet = (from d in GetDebets() where d.ProductId == id select d.Sum).Sum();
            int kredit = (from k in GetKredits() where k.ProductId == id select k.Sum).Sum();
            return begin + debet - kredit;
        }

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
    }
}
