using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InterviewDemo.Models;
using System.Data.SqlClient;

namespace InterviewDemo.Controllers
{
    public class PersonController : Controller
    {
        //SQL Database Connection String
        static string connectString = "Data Source=localhost;Initial Catalog=PersonDB;Integrated Security=True";
        //Initiate PersonDAL
        PersonDAL personData = new PersonDAL(connectString);
                       
        // /person
        public ActionResult Index()
        {
            List<Person> result = personData.getAllPeople();           
            return View(result);
        }

        // /person/id
        public ActionResult Edit(int id)
        {
            Person result = personData.getPerson(id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                personData.update(person);
                return RedirectToAction("Index");
            }
            return View(person);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                personData.create(person);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            Person result = personData.getPerson(id);
            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            personData.delete(id);
            return RedirectToAction("Index");
        }
    }
}