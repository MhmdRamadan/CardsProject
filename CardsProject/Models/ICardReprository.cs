using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsProject.Models
{
   public interface ICardReprository
    {
        IEnumerable<BusinessCard> GetAllCards();
        BusinessCard GetCardById(int id);
        void CreateCard(BusinessCard cardToBeCreated);
        void DeleteCard(int id);
        int SaveChanges();
    }
}
