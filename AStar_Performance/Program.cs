using AStar_Performance;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Running;

//System.Environment.SetEnvironmentVariable("R_HOME", "D:\\R-4.3.0");

var config = ManualConfig.CreateMinimumViable();
config.AddExporter(CsvMeasurementsExporter.Default);
config.AddExporter(RPlotExporter.Default);

//BenchmarkRunner.Run<Md5VsSha256>(config);
//BenchmarkRunner.Run<OtherTest>(config);

//var aStar = new AStar();
//Console.WriteLine(aStar.GenerateMap());
//Console.WriteLine(result);

//var convertTest=new ConvertTest();
//convertTest.ToTileArray();

//BenchmarkRunner.Run<ConvertTest>(config);

//BenchmarkRunner.Run<HashSetTest>(config);

BenchmarkRunner.Run<AStar>(config);
