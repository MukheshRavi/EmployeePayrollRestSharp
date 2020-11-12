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
        public IRestResponse GetEmployeeList()
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
        public void OnCallingGetApi_ReturnEmployeeList()
        {
            //gets the irest response from getemployee list method
            IRestResponse response = GetEmployeeList();
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
        /// <summary>
        /// Givens the employee on post should return added employee. UC2
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            //Arrange
            Employee employee = new Employee { FirstName = "Rashid", salary = "500000" };
            //adding request to post(add) data
            RestRequest request = new RestRequest("/employees", Method.POST);
                //instatiating jObject for adding data for name and salary, id auto increments
                JObject jObject = new JObject();
                jObject.Add("FirstName", employee.FirstName);
                jObject.Add("salary", employee.salary);
                //as parameters are passed as body hence "request body" call is made, in parameter type
                request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                ///Act
                /// response will contain the data which is added and not all the data from jsonserver.
                ///data is added to json server json file in this step.
                IRestResponse response = client.Execute(request);
                ///Assert
                ///code will be 201 for posting data
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
                //derserializing object for assert and checking test case
                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(employee.FirstName, dataResponse.FirstName);
                Console.WriteLine(response.Content);

        }
    }
}
