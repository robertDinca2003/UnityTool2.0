
namespace UnityTool2._0;

abstract class UnityTool
{
    private static string _unityProjectPath = "/home/robert/Desktop/unityTool/TestCase01";
    private static string _outputFolderPath = "/home/robert/Desktop/unityTool/Output";

    private static string[]? _allScenePath ;
    private static string[]? _allScriptPath ;
    
    private static readonly ParserYaml Parser = new ParserYaml();
    private static Hierarchy? _hierarchy;
    private static UnusedScripts? _unusedScripts;

    private static void CreateHierarchy()
    {
        _hierarchy = new Hierarchy
        {
            AllObjects =  Parser.AllObjects,
            AllTransformsList =  Parser.AllTransformsList,
            AllRoots = Parser.AllRoots,
            DestinationFolder = _outputFolderPath,
        };
        if (_allScenePath != null)
            foreach (var scenePath in _allScenePath)
            {
                Task.Run(()=>_hierarchy.NewHierarchy(scenePath));
            }
        Console.WriteLine("All scene hierarchies have been created");

    }

    private static void FindUsedScripts()
    {
        _unusedScripts = new UnusedScripts
        {
            AllMonoBehaviours =  Parser.AllMonoBehaviours,
            ScriptPath = _allScriptPath,
            DestinationFolder = _outputFolderPath,
        };
        _unusedScripts.ScriptFinding();
        Console.WriteLine("All unused scripts have been found!");

    }

    static void ObtainAllScripts()
    {
        _allScriptPath = Directory.GetFiles(Path.Combine(_unityProjectPath,"Assets"), "*.cs", SearchOption.AllDirectories);
        /*foreach (var str in _allScriptPath)
        {
            Console.WriteLine(str);
        }*/
    }

    static void ObtainAllScenes()
    {
        _allScenePath = Directory.GetFiles(Path.Combine(_unityProjectPath, "Assets"), "*.unity",
            SearchOption.AllDirectories);
        /*foreach (var scene in _allScenePath)
        {
            Console.WriteLine(scene);
        }*/
    }

   
    static void GenerateSolution()
    {
        Console.WriteLine("Started to generate solution");
        
        var processScripts=Task.Run(ObtainAllScripts);
        var processScenes= Task.Run(ObtainAllScenes);

        Task.WaitAll(processScenes, processScripts);        
        Parser.ParsingSceneYaml(_allScenePath);
        
        
        
        
        Task.WaitAll(Task.Run(CreateHierarchy),Task.Run(FindUsedScripts));
        Console.WriteLine("The process is done!");
    }
    
    static bool VerifyInput(string?[] args)
    {
        Console.WriteLine("Verifying Input..");
        if (args.Length == 0)
        {
            Console.WriteLine("Error: No arguments given.\nCommand: ./tool.exe projectPath outputPath");
            return true;
        }

        if (args.Length == 1)
        {
            Console.WriteLine("Error: Not enough arguments given.\nCommand: ./tool.exe projectPath outputPath");
            return true;
        }

        if (args.Length > 2)
        {
            Console.WriteLine("Error: Too many arguments given.\nCommand: ./tool.exe projectPath outputPath");
            return true;
        }

        if (!Directory.Exists(args[0]))
        {
            Console.WriteLine("Error: Unity project directory do not exist: "+args[0]);
            if (!Directory.Exists(args[1]))
            {
                Console.WriteLine("Error: Output directory do not exist: "+ args[1]);
            }
            return true;
        }

        if (!Directory.Exists(args[1]))
        {
            Console.WriteLine("Error: Output directory do not exist: "+ args[1]);
            return true;
        }

        if (!Directory.Exists(Path.Combine(args[0] ?? string.Empty, "Assets")))
        {
            Console.WriteLine("Error: Do not exist an Assets directory in "+args[0]);
            return true;
        }
        
        Console.WriteLine("Correct Input");
        return false;
    }


    static void Main(string?[] args)
    {
        //Stopwatch stopwatch = new Stopwatch();

        Console.WriteLine("Starting the program...");
        
        // Start the stopwatch
        //stopwatch.Start();
        
        if(VerifyInput(args))
            return;
        _unityProjectPath = args[0] ?? throw new InvalidOperationException();
        _outputFolderPath = args[1] ?? throw new InvalidOperationException();
        GenerateSolution();
        
        //stopwatch.Stop();
       
        Console.WriteLine("Program finished.");
        Console.ReadKey();
        // Get the elapsed time and display it
        //TimeSpan elapsed = stopwatch.Elapsed;
        //Console.WriteLine($"Elapsed time: {elapsed.Milliseconds}");
    }    
}