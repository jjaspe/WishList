using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishList.Models;
using System.Data;
using System.Data.Entity;

namespace WishList.Tests.FakeContexts
{
    public class FakeEntry:IDBEntry
    {
        EntityState state;
        System.Data.EntityState IDBEntry.State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        object item;

        public object Item
        {
            get { return item; }
            set { item = value; }
        }
    }

    public class FakeWishListDBContext:IWishListDBContext
    {
        List<object> Added = new List<object>();
        List<object> Updated = new List<object>();
        List<object> Removed = new List<object>();
        Stack<object> pendingAdded = new Stack<object>(), 
            pendingUpdated = new Stack<object>(), 
            pendingRemoved = new Stack<object>();
        bool Saved = false;


        public IQueryable<T> Query<T>() where T : class
        {
            return Entities.OfType<T>() as IQueryable<T>;
        }

        public IDBEntry Entry(object item)
        {
            return Entries.SingleOrDefault(c => c.Item == item);
        }

        public void Add<T>(T item) where T : class
        {
            pendingAdded.Push(item);
            Saved = false;
        }

        public void Update<T>(T item) where T : class
        {
            pendingUpdated.Push(item);
            Saved = false;
        }

        public void Remove<T>(T item) where T : class
        {
            pendingRemoved.Push(item);
            Saved = false;
        }

        public void SaveChanges()
        {
            while (pendingAdded.Peek() != null)
            {
                IDBEntry entry = new FakeEntry(){Item=pendingAdded.Pop()};
                Entries.Add(entry);
                Entities.Add(entry.Item.GetType(), entry.Item);
            }
                
            //Here we would need to update
            while (pendingUpdated.Peek() != null)
                Updated.Add(pendingUpdated.Pop());

            while (pendingRemoved.Peek() != null)
            {
                IDBEntry entry = Entries.SingleOrDefault(c => c.Item == pendingRemoved.Pop());
                if(entry!=null)
                {
                    Removed.Add(entry.Item);
                    Entries.Remove(entry);
                    //Not removing yet
                }
                    
            }

            Saved = true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        Dictionary<Type , object> Entities = new Dictionary<Type, object>();
        List<IDBEntry> Entries = new List<IDBEntry>();
        
        
    }
}
