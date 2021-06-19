using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardsProject.Models
{
    public class CardReprository : ICardReprository
    {
        private BusinessCardEntities db = new BusinessCardEntities();
        public IEnumerable<BusinessCard> GetAllCards()
        {
            return db.BusinessCards.ToList();
        }

        public void CreateCard(BusinessCard cardToBeCreated)
        {
            db.BusinessCards.Add(cardToBeCreated);
            db.SaveChanges();
        }

        public void DeleteCard(int id)
        {
            var CardToBeDeleted = GetCardById(id);
            db.BusinessCards.Remove(CardToBeDeleted);
            db.SaveChanges();
        }


        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public BusinessCard GetCardById(int id)
        {
            return db.BusinessCards.FirstOrDefault(db => db.ID == id);
        }
    }
}