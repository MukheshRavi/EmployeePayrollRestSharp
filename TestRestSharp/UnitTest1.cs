using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using EmployeePayrollRestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

namespace TestRestSharp
{
    [TestClass]
    public class UnitTest1
    {
        //declaring restclient variable
        RestClient client;
        /// <summary>
        /// Setups this instance for the client by giving url along with port.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }
        /// <summary>
        /// Gets the employee list in the form of irestresponse. 
        /// </summary>
        /// <returns>IRestResponse response</returns>
        public IRestResponse getEmployeeList()
        {
            //arrange
            //makes restrequest for getting all the data from json server by giving table name and method.get
            RestRequest request = new RestRequest("/employees", Method.GET);

            //act
            //executing the request using client and saving the result in IrestResponse.
            IRestResponse response = client.Execute(request);
            return response;
            
        }
        /// <summary>
        /// Ons the calling get API return employee list.
        /// </summary>
        [TestMethod]
        public void onCallingGetApi_ReturnEmployeeList()
        {
            //gets the irest response from getemployee list method
            IRestResponse response = getEmployeeList();
            //assert
            //assert for checking status code of get
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            //adding the data into list from irestresponse by using deserializing.
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            //printing out the content for list of employee
            foreach (Employee employee in dataResponse)
            {
                Console.WriteLine("Id: " + employee.id + " Name: " + employee.FirstName + " Salary: " + employee.salary);
            }
            //assert for checking count of no of element in list to be equal to data in jsonserver table.
            Assert.AreEqual(4, dataResponse.Count);
        }
    }
}
