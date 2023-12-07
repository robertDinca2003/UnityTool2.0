
namespace UnityTool2._0;

public class GameObject
{
    public string MName = "";
    public string ObjId = "";
    public readonly List<string> Components = new List<string>();
}

public class Transform
{
    public string TransId = "";
    public string MGameObject = "";
    public string MFather = "";
    public readonly List<string> MChildren = new List<string>();
}

public class MonoBehaviour
{
    public string MonoId = "";
    public string MGameObject = "";
    public string Guid = "";
    public readonly List<string> MComponents = new List<string>();
}

public class SceneRoots
{

    public readonly Dictionary<string, List<string>> Roots = new Dictionary<string, List<string>>();
}


public class ParserYaml
{
    public readonly Dictionary<string, GameObject> AllObjects = new Dictionary<string, GameObject>();
    public readonly Dictionary<string, Transform> AllTransformsList = new Dictionary<string, Transform>();
    public readonly Dictionary<string, MonoBehaviour> AllMonoBehaviours = new Dictionary<string, MonoBehaviour>();
    public readonly SceneRoots AllRoots = new SceneRoots();

    
    
    private void SceneParsing(string path)
    {
        string[] code = File.ReadAllLines(@path);

        
        
        bool isGameObject = false;
        bool isTransform = false;
        bool isMono = false;
        bool isSceneRoot = false;
        bool hasChildren = false;

        string lastId = "";
        
        foreach (string line in code)
        {
            if (line.Contains("---"))
            {
                hasChildren = false;
                isTransform = false;
                isMono = false;
            }
            if (line.Length>8 && line.Substring(0, 9) == "--- !u!1 ")
            {
                GameObject newObj = new GameObject
                {
                    ObjId = line.Substring(line.IndexOf('&') + 1)
                };
                isGameObject = true;
                isTransform = false;
                isMono = false;
                lastId = newObj.ObjId;
                AllObjects.TryAdd(newObj.ObjId,newObj);
                continue;
            }

            if (line.Length>8 && isGameObject && line.Substring(0, 9) == "--- !u!4 ")
            {
                Transform newTransform = new Transform();
                isTransform = true;
                isMono = false;
                newTransform.TransId = line.Substring(line.IndexOf('&') + 1);
                lastId = newTransform.TransId;
                AllTransformsList.TryAdd(newTransform.TransId,newTransform);
                continue;
            }
            
            if (line.Length>10 && isGameObject && line.Substring(0, 11) == "--- !u!114 ")
            {
                MonoBehaviour newMonoList = new MonoBehaviour();
                isTransform = false;
                isMono = true;
                newMonoList.MonoId = line.Substring(line.IndexOf('&') + 1);
                lastId = newMonoList.MonoId;
                AllMonoBehaviours.Add(newMonoList.MonoId,newMonoList);
                continue;
            }
            
            if (line.Contains("SceneRoots:"))
            {isSceneRoot = true;
                isGameObject = false;
                isTransform = false;
                isMono = false;
                continue;
            }

            if (isGameObject && !isTransform && !isMono)
            {
                if (line.Contains("m_Name"))
                {
                    string temp = line.Substring(line.IndexOf("m_Name: ", StringComparison.Ordinal)+8);
                    AllObjects[lastId].MName =temp;
                }

                if (line.Contains("component: "))
                {
                    string temp = line.Substring(line.IndexOf("fileID: ", StringComparison.Ordinal)+8);
                    AllObjects[lastId].Components.Add(temp.Substring(0,temp.Length-1));
                }
            }

            if (isTransform && isGameObject && !isMono)
            {
                if (line.Contains("m_Children:"))
                {
                    hasChildren = true;
                }
                if (line.Contains("m_Children: []"))
                {
                    hasChildren = false;
                    continue;
                }
                if (line.Contains("m_Father: "))
                {
                    hasChildren = false;
                    string temp = line.Substring(line.IndexOf("fileID: ", StringComparison.Ordinal) + 8);
                    AllTransformsList[lastId].MFather = temp.Substring(0, temp.Length - 1);
                }
                if (hasChildren && line.Contains("fileID: "))
                {
                    string temp = line.Substring(line.IndexOf("fileID: ", StringComparison.Ordinal) + 8);
                    AllTransformsList[lastId].MChildren.Add(temp.Substring(0,temp.Length-1));
                }
                
                

                if (line.Contains("m_GameObject: "))
                {
                    string temp = line.Substring(line.IndexOf("fileID: ", StringComparison.Ordinal) + 8);
                    AllTransformsList[lastId].MGameObject = temp.Substring(0, temp.Length - 1);
                }
            }

            if (isMono && isGameObject && !isTransform)
            {
                if (line.Contains("m_GameObject: "))
                {
                    string temp = line.Substring(line.IndexOf("fileID: ", StringComparison.Ordinal) + 8);
                    AllMonoBehaviours[lastId].MGameObject = temp.Substring(0, temp.Length - 1);
                    continue;
                }

                if (line.Contains("m_Script: "))
                {
                    string temp = line.Substring(line.IndexOf("guid: ", StringComparison.Ordinal) + 6);
                    temp = temp.Substring(0, temp.IndexOf(','));
                    AllMonoBehaviours[lastId].Guid = temp;
                    continue;
                }

                if (line.Contains("m_EditorClassIdentifier:"))
                {
                    hasChildren = true;
                    continue;
                }

                if (hasChildren)
                {
                    string temp = line.Substring(0,line.IndexOf(':'));
                    temp = temp.Substring(temp.LastIndexOf(' ') + 1);
                    AllMonoBehaviours[lastId].MComponents.Add(temp);
                    continue;
                }
                
            }
            
            if (isSceneRoot)
            {
                if (line.Contains("fileID: "))
                {
                    string temp = line.Substring(line.IndexOf("fileID: ", StringComparison.Ordinal)+8);
                    if (!AllRoots.Roots.ContainsKey(path))
                        AllRoots.Roots.TryAdd(path, new List<string>());
                    AllRoots.Roots[path].Add(temp.Substring(0,temp.Length-1));

                }
            }
        }

    }
    public void ParsingSceneYaml(string[]? paths)
    {
        if (paths == null) return;
        foreach (var path in paths)
        {
            SceneParsing(path);
        }
    }
}