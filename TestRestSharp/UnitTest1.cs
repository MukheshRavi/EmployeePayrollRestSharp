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
            Assert.AreEqual(3, dataResponse.Count);
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
        /// <summary>
        /// UC3
        /// Givens the employee on post should return added employee
        /// </summary>
        [TestMethod]
        public void GivenEmployeeList_OnPost_ShouldReturnAddedEmployee()
        {
            //adding multiple employees to table
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { FirstName = "David", salary = "600000" });
            employeeList.Add(new Employee { FirstName = "Kane", salary = "500000" });
            employeeList.ForEach(employeeData =>
            {
                //arrange
                //adding request to post(add) data
                RestRequest request = new RestRequest("/employees", Method.POST);

                //instatiating jObject for adding data for name and salary, id auto increments
                JObject jObject = new JObject();
                jObject.Add("FirstName", employeeData.FirstName);
                jObject.Add("salary", employeeData.salary);
                //as parameters are passed as body hence "request body" call is made, in parameter type
                request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                //Act
                //request contains method of post and along with added parameter which contains data to be added
                //hence response will contain the data which is added and not all the data from jsonserver.
                //data is added to json server json file in this step.
                IRestResponse response = client.Execute(request);
                //assert
                //code will be 201 for posting data
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
                //derserializing object for assert and checking test case
                IRestResponse response1 = GetEmployeeList();
                List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response1.Content);
                Assert.AreEqual(6, dataResponse.Count);
                Console.WriteLine(response.Content);
            });

        }
        /// <summary>
        /// Givens the employee on update should return updated employee. UC4
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnUpdate_ShouldReturnUpdatedEmployee()
        {
            //making a request for a particular employee to be updated
            RestRequest request = new RestRequest("employees/1", Method.PUT);
            //creating a jobject for new data to be added in place of old
            JObject jobject = new JObject();
            jobject.Add("FirstName","Manish");
            jobject.Add("salary", 500000);
            //adding parameters in request
            //request body parameter type signifies values added using add.
            request.AddParameter("application/json", jobject, ParameterType.RequestBody);
            //executing request using client
            IRestResponse response = client.Execute(request);
            //checking status code of response
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            //deserializing content added in json file
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            //asserting for salary
            Assert.AreEqual(dataResponse.salary, "500000");
            //writing content without deserializing from resopnse. 
            Console.WriteLine(response.Content);
        }
    }
}
