using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardsProject.Models;

namespace HomeControllerTest.Reprository
{
    class InMemoryCardRepository : ICardReprository
    {
        private List<BusinessCard> db = new List<BusinessCard>();
        public IEnumerable<BusinessCard> GetAllCards()
        {
            return db.ToList();
        }

        public void CreateCard(BusinessCard cardToBeCreated)
        {
            db.Add(cardToBeCreated);
          
        }
        public void Add(BusinessCard cardToBeAdded)
        {
           db.Add(cardToBeAdded);
        }

        public void DeleteCard(int id)
        {
            var CardToBeDeleted = GetCardById(id);
            db.Remove(CardToBeDeleted);
           
        }

      
        public int SaveChanges()
        {
            return 1;
        }

        public BusinessCard GetCardById(int id)
        {
            return db.FirstOrDefault(db => db.ID == id);
        }
    }
}
