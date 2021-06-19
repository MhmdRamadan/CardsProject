using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using CardsProject.Models;

namespace CardsProject.Controllers
{
    public class HomeController : Controller
    {
        ICardReprository _repository;
        public HomeController() : this(new CardReprository()) { }
        BusinessCardEntities db = new BusinessCardEntities();
        public HomeController(ICardReprository reprository)
        {
            _repository = reprository;
        }
        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult Index1()
        {
            return View("Index1");
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public ActionResult CreateCard(BusinessCard card)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            if (ModelState.IsValid)
            {
                BusinessCard NewCard = new BusinessCard();
                NewCard.Name = card.Name;
                NewCard.Gender = card.Gender;
                NewCard.DOB = card.DOB;
                NewCard.Phone = card.Phone;
                NewCard.Address = card.Address;
                NewCard.Email = card.Email;
              

                string base64String = Convert.ToBase64String(ConvertToBytes(file));
                NewCard.Photo = base64String;
                db.BusinessCards.Add(NewCard);
                db.SaveChanges();
                TempData["CreatedCard"] = " ";
                return RedirectToAction("CardDetails");
            }
            return RedirectToAction("Create"); 
        }
        public ActionResult XMlCreateCard()
        {
            try
            {
                BusinessCard card = (BusinessCard)TempData["CreateCard"];
                BusinessCard NewCard = new BusinessCard();
                NewCard.Name = card.Name;
                NewCard.Gender = card.Gender;
                NewCard.DOB = card.DOB;
                NewCard.Phone = card.Phone;
                NewCard.Address = card.Address;
                NewCard.Email = card.Email;
                db.BusinessCards.Add(NewCard);
                db.SaveChanges();
                TempData["CreatedCard"] = " ";
                return RedirectToAction("CardDetails");
            }catch
            {
                return RedirectToAction("XmlView");
            }

          
        }
        public ActionResult CsvCreateCard()
        {
            try
            {
                BusinessCard card = (BusinessCard)TempData["CsvCard"];
                BusinessCard NewCard = new BusinessCard();
                NewCard.Name = card.Name;
                NewCard.Gender = card.Gender;
                NewCard.DOB = card.DOB;
                NewCard.Phone = card.Phone;
                NewCard.Address = card.Address;
                NewCard.Email = card.Email;

                db.BusinessCards.Add(NewCard);
                db.SaveChanges();
                TempData["CreatedCard"] = " ";
                return RedirectToAction("CardDetails");
            }catch
            {
                return RedirectToAction("ImportCsv");
            }


        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
        //Get all cards
        public ViewResult CardDetails()
        {
            ViewData["ControllerName"] = this.ToString();
            List<BusinessCard> cards = db.BusinessCards.ToList();
            return View("CardDetails", _repository.GetAllCards());
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var card = db.BusinessCards.Where(x => x.ID == id).FirstOrDefault();
            db.BusinessCards.Remove(card);
            db.SaveChanges();
            TempData["DeletedCard"] = " ";
            return RedirectToAction("CardDetails");
        }
        public FileResult DownloadXml(int id)
        {
            var model = db.BusinessCards.Where(x => x.ID == id).FirstOrDefault();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(model.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, model,ns);
                xmlStream.Position = 0;
                //Loads the XML document from the specified string.
                xmlDoc.Load(xmlStream);
               
            }
            string filename = model.Name + ".xml";

            return File(Encoding.UTF8.GetBytes(xmlDoc.InnerXml), "application/xml", filename);
        }

        public FileResult Export(int id)
        {
            List<object> cards = new List<object>();
            var card = db.BusinessCards.Where(x => x.ID == id).FirstOrDefault();
          
            cards.Add(card);
           
            //Insert the Column Names.
            cards.Insert(0, new string[6] { "ID", "Name", "Gender", "Date of birth","phone","photo" });

            StringBuilder sb = new StringBuilder();

           
            sb.Append("Name ,");
            sb.Append("Gender ,");
            sb.Append("DOB ,");
            sb.Append("Email ,");
            sb.Append("Phone ,");
            sb.Append("Photo ,");
            sb.Append("Address ,");
            sb.Append("\r\n");
            sb.Append("" + card.Name + ",");
            sb.Append("" + card.Gender + ",");
            sb.Append("" + card.DOB + ",");
            sb.Append("" + card.Email + ",");
            sb.Append("" + card.Phone + ",");
            sb.Append("" + card.Photo + ",");
            sb.Append("" + card.Address + ",");
          
            // sb.Append(new string[6] { card.Name, card.Gender, ""+card.DateOfBirth,""+card.Phone,""+card.Phone });
            string filename = card.Name + ".csv";

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", filename);
        }
        public ActionResult XmlView()
        {
            return View("XmlView");
        }
        [HttpPost]
        public ActionResult UploadXml(HttpPostedFileBase file)
        {
            
                 List<BusinessCard> Cardlist = new List<BusinessCard>();
            try
            {
                var xmlFile = file;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFile.InputStream);
                XmlNodeList CardNodes = xmlDocument.SelectNodes("BusinessCard");
                foreach (XmlNode XmlCard in CardNodes)
                {
                    String phone = XmlCard["Phone"].InnerText;
                    string date = XmlCard["DOB"].InnerText;
                   
                    Cardlist.Add(new BusinessCard()
                    {

                        Name = XmlCard["Name"].InnerText,
                        Gender = XmlCard["Gender"].InnerText,
                        Phone = XmlCard["Phone"].InnerText,
                       DOB= XmlCard["DOB"].InnerText,
                        Address = XmlCard["Address"].InnerText,
                        Email=XmlCard["Email"].InnerText




                    });
                    TempData["CreateCard"] = Cardlist[0];

                    TempData["Card"] = Cardlist[0];
                }



                //
                return RedirectToAction("SingleCardDetails");
            }catch
            {
                TempData["XmlError"] = " ";
                return RedirectToAction("XmlView");
            }
            }
        public ActionResult SingleCardDetails()
        {
            BusinessCard card = (BusinessCard)TempData["Card"];
            return View("SingleCardDetails", card);
        }
        public ActionResult ImportCsv ()
        {
            return View("ImportCsv");
        }
        [HttpPost]
        public ActionResult UploadCsv(HttpPostedFileBase postedFile)
        {
            List<BusinessCard> cards = new List<BusinessCard>();
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    //Read the contents of CSV file.
                    string csvData = System.IO.File.ReadAllText(filePath);

                    //Execute a loop over the rows.
                    int flag = 0;
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (flag != 0)
                        {

                          
                            if (!string.IsNullOrEmpty(row))
                            {
                                cards.Add(new BusinessCard
                                {

                                    Name = row.Split(',')[0],
                                    Gender = row.Split(',')[1],
                                    Phone = row.Split(',')[4],
                                    DOB = row.Split(',')[2],
                                    Address =row.Split(',')[6] ,
                                    Email=row.Split(',')[3],
                                    Photo=row.Split(',')[5]

                                });
                            }
                        }
                        flag = 1;
                    }
                    TempData["CsvCard"] = cards[0];
                }

                return View(cards);
            }catch
            {
                TempData["CsvError"] = " ";
                return RedirectToAction("ImportCsv");
            }
        }
    }

    }
