// See https://aka.ms/new-console-template for more information

using CsvHelper.Configuration;
using Linq2dbContext.DAL;
using Linq2dbContext.Helpers;
using Linq2dbContext.Model;
using CsvHelper.Configuration.Attributes;
var db = new DBContext("Data Source=172.16.5.81\\SQL2012;Database=LVERP;User Id=sa;Password=erd01123456120; pooling=false;TrustServerCertificate=True", "SqlServer");

int a = db.Query<AD_Users>().Count();
var b = ReadData.ReadCsv<tempModel>("D:\\test.csv");
Console.WriteLine("Test connection {0}", a);

public class tempModel
{
	public string Id { get; set; }

	[Name("dispatch_id")]
	public string DispatchId { get; set; }

	[Name("prediction_date")]
	public string PredictionDate { get; set; }

	[Name("repeat_flag")]
	public string RepeatFlag { get; set; }

	[Name("prob_repeat_dispatch")]
	public string ProbRepeatDispatch { get; set; }

	[Name("rca_repeat_dispatch")]
	public string RCARepeatDispatch { get; set; }

	[Name("tech_recommendation_ids")]
	public string TechRecommendationIds { get; set; }
}
