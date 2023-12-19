# UnityTool2.0
UnityTool2.0 is a console application that can receive 2 parameters, a unity project folder, and an output folder. The tool will return the hierarchy for every scene and all the unused scripts in the output folder.

I recommend reading at least the part about the problems with the task and backward compatibility.

I have completed the main task and the second bonus task(about the parallelizing).

# Resources I have read and helped in the development:
- https://docs.unity3d.com/Manual/Hierarchy.html (general info about scenes)
- https://docs.unity3d.com/Manual/FormatDescription.html (format of a component in scene yalm)
- https://docs.unity3d.com/Manual/ClassIDReference.html (yalm class id)
- https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.waitall?view=net-8.0 (for parallelizing)

# The algorithm is described as simple and efficient as possible:
- Step 1: Verify the input
    - The application will not start if the input has more or less than 2 parameters.
    - The application will not start if the unity project folder does not contain an assets folder as a direct subfolder.
    - The application will not start if the directories do not exist.
    - The user will be notified about any of the above
- Step 2: Scraping the unity project and collect all scripts and scenes
- Step 3: Parse all the scenes and create a dictionary with the keys being the class id and the values of the actual properties I need.
    - GameObject
    - Transform
    - MonoBehaviour
    - SceneRoots
- Step 4: Creating the hierarchy
    - Step 4.1: For each scene, I have to access its scene root which consists of IDs of the Transform class.
    - Step 4.2: With the help of the property m_gameobject, is possible to access by id a certain GameObject and get its name.
    - Step 4.3: For every child of the current Transform, step 4 will be applied, creating in that way a DFS.
- Step 5: Finding the unused scripts:
    - Step 5.1: Generate a hash set with all guid of the used scripts which can be found in the MonoBehaviour objects generated in step 3
    - Step 5.2: For every  script collected in step 2, I will extract its guid from the metafile, and verify if it is in the set
    - Step 5.3: If the script is not found in the set, that means it is unused

  # Problems with the task and tool
  I observed that in the version of Unity 2021 and below the SceneRoots attribute from the scene yalm file does not exist. That means there is a compatibility issue.
  I discovered that by applying the tool to an old project.
  The unity documentation contains such example with a scene yalm example which has been carried for years and not updated:
  https://docs.unity3d.com/2021.3/Documentation/Manual/YAMLSceneExample.html
  
  Also in old unity projects like the following one, the SceneRoot attribute is missing:
  
  https://github.com/nvjob/Infinity-Square-Space/blob/master/Assets/OSG%20Infinity%20Square%20Space/Scenes/main.unity

  # The optimization added by parallelizing

  The time shown I know is not the real-time that the application spends on the CPU, but I believe is an interesting statistic.

  I have also built a version without any parallelized process which is around 2 times slower than the current version.
  I used a stopwatch to count the differences in both scripts.

  The optimized version scoring 10 milliseconds
  ![Screenshot_20231207_175601](https://github.com/robertDinca2003/UnityTool2.0/assets/71851178/dc934fa5-4df0-4552-8149-f3cb64b4f210)

  The unoptimized version scoring between 19-20 milliseconds
  ![Screenshot_20231207_180220](https://github.com/robertDinca2003/UnityTool2.0/assets/71851178/6be585e9-27bf-4a6d-bcb8-78ba5b29af59)

  
  

