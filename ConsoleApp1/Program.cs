using System.Net.Http.Json;
using WebApplication1;

class Program
{
    static void Main()
    {
        HttpClient client = new HttpClient( );
        client.BaseAddress = new Uri("http://localhost:5204");
        void viewdetail()
        {
            string url = "http://localhost:5204/api/Car/2";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }

        void alldetails()
        {
            
            string url = "http://localhost:5204/api/Cars";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.WriteLine(data1); 
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(ans);
        }

        void addDetail()
        {
            Car temp = new Car() {Name = "test2", SitCounter = 123};
            
            Task<HttpResponseMessage> request =  client.PostAsJsonAsync(
                $"api/Car", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }

        void changedetail()
        {
            Car temp = new Car() {Name = "test3", SitCounter = 321, Id = 3};
            
            Task<HttpResponseMessage> request =  client.PutAsJsonAsync(
                $"api/Car", temp);
            string ans = request.Result.StatusCode.ToString();
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            Console.Write(ans);
        }

        void deletedetail()
        {
            
            string url = "http://localhost:5204/api/Car/2";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Delete, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }
        
        void viewAssembly()
        {
            
            string url = "http://localhost:5204/api/TaxiDepot/1";
            Task<HttpResponseMessage> request = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }
           
        void allassemblies()
        {
            
            string url = "http://localhost:5204/api/TaxiDepots";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.WriteLine(data1); 
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }
        void addassembly()
        {
            
            TaxiGroupView a = new TaxiGroupView() {DetailName = "test1", DetailId = 1, SitCounter = 5};
            TaxiGroupView b = new TaxiGroupView() {DetailName = "test2", DetailId = 2, SitCounter = 5};
            TaxiDepotView temp = new TaxiDepotView() {name = "test1", TaxiGroups = {a, b}};
            Task<HttpResponseMessage> request =  client.PostAsJsonAsync(
                $"api/TaxiDepot", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }
        
        void changessembly()
        {
            
            TaxiGroupView a = new TaxiGroupView() {DetailName = "test1", DetailId = 1, SitCounter = 12};
            TaxiGroupView b = new TaxiGroupView() {DetailName = "test2", DetailId = 2, SitCounter = 12};
            TaxiDepotView temp = new TaxiDepotView() {name = "test1", TaxiGroups = {a, b}, id = 1};
            Task<HttpResponseMessage> request =  client.PutAsJsonAsync(
                $"api/TaxiDepot", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }
        
        void deleteassembly()
        {
            
            string url = "http://localhost:5204/api/TaxiDepot/2";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Delete, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(ans);
        }
        //alldetails();
        //viewdetail();
        //addDetail();
        //deletedetail();
        //viewdetail();
        //alldetails();
        //alldetails();
        //deleteassembly();
        //addassembly();
        //viewAssembly();
        allassemblies();
        //changessembly();
    }
}