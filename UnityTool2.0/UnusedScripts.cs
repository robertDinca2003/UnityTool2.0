namespace UnityTool2._0;

public class UnusedScripts
{
    public Dictionary<string, MonoBehaviour> AllMonoBehaviours = new Dictionary<string, MonoBehaviour>();
    public string[]? ScriptPath ;
    public string DestinationFolder = "";
    public void ScriptFinding()
    {
        HashSet<string> allGuid = new HashSet<string>();
        List<string> unusedScripts = new List<string>();
        if (unusedScripts == null) throw new ArgumentNullException(nameof(unusedScripts));

        foreach (var variable in AllMonoBehaviours.Keys)
        {
            //Console.WriteLine(_allMonoBehaviours[VARIABLE].Guid);
            allGuid.Add(AllMonoBehaviours[variable].Guid);
        }

        if (ScriptPath != null)
            foreach (var path in ScriptPath)
            {
                string code = File.ReadAllText(path + ".meta");
                string temp = code.Substring(code.IndexOf("guid: ", StringComparison.Ordinal) + 6);

                temp = temp.Substring(0, temp.IndexOf('\n', StringComparison.Ordinal));
                //Console.WriteLine(temp);
                if (!allGuid.Contains(temp))
                {
                    unusedScripts.Add(path);
                    //Console.WriteLine(path);
                }
            }

        string message = "Relative Path,GUID\n";
        foreach (var script in unusedScripts)
        {
            message += script.Substring(script.IndexOf("Assets", StringComparison.Ordinal));
            message += ',';
            string temp = File.ReadAllText(script+".meta"); 
            temp = temp.Substring(temp.IndexOf("guid:", StringComparison.Ordinal) + 6);
            temp = temp.Substring(0, temp.IndexOf('\n'));
            message += temp + '\n';
        }
        //Console.WriteLine("="+message);
        File.WriteAllText(Path.Combine(DestinationFolder,"UnusedScripts.txt"),message);
    }
}