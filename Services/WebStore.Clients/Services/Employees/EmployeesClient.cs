using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
namespace WebStore.Clients.Services.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(HttpClient client) : base(client, WebAPIAddresses.Employees)
        {
          //  ServiceAddress = "api/employees";
        }
     //   protected sealed override string ServiceAddress { get; set; }

        public IEnumerable<EmployeeViewModel> GetAll()
        {
            var url = $"{ServiceAddress}";
            var list = Get<List<EmployeeViewModel>>(url);
            return list;
        }

        public EmployeeViewModel GetById(int id)
        {
            var url = $"{ServiceAddress}/{id}";
            var result = Get<EmployeeViewModel>(url);
            return result;
        }

        public EmployeeViewModel Update(EmployeeViewModel entity)
        {
            var url = $"{ServiceAddress}/{entity.Id}";
            var response = Put(url, entity);
            var result = response.Content.ReadAsAsync<EmployeeViewModel>().Result;
            return result;
        }

        public int Add(EmployeeViewModel model)
        {
            var url = $"{ServiceAddress}";
            var result = Post(url, model);
            return result.Content.ReadAsAsync<int>().Result;
        }

        public bool Delete(int id)
        {
            var url = $"{ServiceAddress}/{id}";
            var result = Delete(url);
            return result.Content.ReadAsAsync<bool>().Result;
        }

        public void Commit()
        {
        }
    }
}
