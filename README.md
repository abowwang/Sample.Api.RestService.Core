# Example

## MyRestService
```csharp
public class MyRestService : RestServiceJson{
    public readonly string POST_API = "/v1/api/path";
    
    public MyRestService WithDomain()
    {
        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

        Dictionary<string,string> dict = config.GetSection($"{section name}").Get<Dictionary<string, string>>();
        this.reqDomain = dict["domain"];
        this.fixedHeaders.Add("app-id",dict["app_id"]);
        return this;
    }

    public MyRestService WithMemberToken(String memberToken)
    {
        this.fixedHeaders.Add("member-token",memberToken);
        return this;
    }
}
```

## MyRestResp
```csharp

public class RestServiceResp<T>
{
    public int retCode {get;set;} = 1;
    public string retMessage {get;set;} = "";
    public T result {get;set;}
}

public class RestServiceResp
{
    public int retCode {get;set;} = 1;
    public string retMessage {get;set;} = "";
}

public class ListResult<T>
{
    public int? page_index { get;set; } = 0;
    public int? page_size {get;set;} = 0;
    public int? total_records { get;set; } = 0;
    public List<T> list {get;set;} = new List<T>();
}
```

### Call Service
```csharp
MyRestService service =  new MyRestService().WithDomain().WithMemberToken(access_token);
Dictionary<String, String> dictHeader = new Dictionary<String, String> {
    { "header-name", $"{header_value}" }
};
RestServiceResp<ResultModel> respProductPortfolio = service.sendRequest<RestServiceResp<ResultModel>>(service.POST_API, "POST", null, dictHeader, null);
```