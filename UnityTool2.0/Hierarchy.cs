namespace UnityTool2._0;

public class Hierarchy
{
    public  Dictionary<string, GameObject> AllObjects = new Dictionary<string, GameObject>();
    public Dictionary<string, Transform> AllTransformsList = new Dictionary<string, Transform>();
    public  SceneRoots AllRoots = new SceneRoots();
    public string DestinationFolder = "";
    private readonly Dictionary<string, string> _allHierarchies = new Dictionary<string, string>();
    
    private void GenerateDfs(string node, int depth,string str)
    {
        for (int space = 0; space < depth*2; space++)
        {
            //Console.Write('-');
            _allHierarchies[str] += '-';

        }

        _allHierarchies[str] += AllObjects[AllTransformsList[node].MGameObject].MName + '\n';
        //Console.WriteLine(_allObjects[_allTransformsList[node].MGameObject].MName);
        foreach (var child in AllTransformsList[node].MChildren)
        {
            GenerateDfs(child,depth+1,str);
        }
    }
    public void NewHierarchy(string scene)
    {
        _allHierarchies.TryAdd(scene,"");
        foreach (var variableRoot in AllRoots.Roots[scene])
        {
            GenerateDfs(variableRoot,0,scene);
            
        }
        //Console.WriteLine(_allHierarchies[scene]);
        FileInfo nameFile = new FileInfo(scene);
        //Console.WriteLine(Path.Combine(DestinationFolder,nameFile.Name+".dump"));
        File.WriteAllText(Path.Combine(DestinationFolder,nameFile.Name+".dump"), _allHierarchies[scene]);
    }
}