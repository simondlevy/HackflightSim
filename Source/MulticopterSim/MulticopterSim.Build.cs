/*
* MulticopterSimBuild.cs: Build script for MulticopterSim
*
* Copyright (C) 2018 Simon D. Levy
*
* MIT License
*/


using UnrealBuildTool;
using System;
using System.IO;

public class MulticopterSim : ModuleRules
{
	public MulticopterSim(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore" });

        PrivateIncludePaths.Add(Target.Platform == UnrealTargetPlatform.Win64 ?
            Environment.GetEnvironmentVariable("userprofile") + "\\Documents\\Arduino\\libraries\\Hackflight\\src" :
            Environment.GetEnvironmentVariable("HOME") + "/Documents/Arduino/libraries/Hackflight/src");

        // OpenCV (currently Windows-only
        if (Target.Platform == UnrealTargetPlatform.Win64) {
            LoadOpenCV(Target);
        }

        //LoadPython(Target);
    }

    private string ThirdPartyPath
    {
        get { return Path.GetFullPath(Path.Combine(ModuleDirectory, "../../ThirdParty/")); }
    }

    public void LoadOpenCV(ReadOnlyTargetRules Target)
    {
        // Start OpenCV linking here!
        bool isLibrarySupported = false;

        // Create OpenCV Path 
        string OpenCVPath = Path.Combine(ThirdPartyPath, "OpenCV");

        // Get Library Path 
        string LibPath = "";
        bool isdebug = Target.Configuration == UnrealTargetConfiguration.Debug && Target.bDebugBuildsActuallyUseDebugCRT;
        LibPath = Path.Combine(OpenCVPath, "lib");
        isLibrarySupported = true;

        //Add Include path 
        PublicIncludePaths.AddRange(new string[] { Path.Combine(OpenCVPath, "include") });

        // Add Library Path 
        PublicLibraryPaths.Add(LibPath);

        //Add Static Libraries
        PublicAdditionalLibraries.Add("opencv_world340.lib");

        //Add Dynamic Libraries
        PublicDelayLoadDLLs.Add("opencv_world340.dll");
        PublicDefinitions.Add(string.Format("WITH_OPENCV_BINDING={0}", isLibrarySupported ? 1 : 0));
    }

    public bool LoadPython(ReadOnlyTargetRules Target)
    {
        // Start OpenCV linking here!
        bool isLibrarySupported = false;

        // Create OpenCV Path 
        string PythonPath = Environment.GetEnvironmentVariable("userprofile") + "\\AppData\\Local\\Programs\\Python\\Python36";

        // Get Library Path 
        string LibPath = "";
        bool isdebug = Target.Configuration == UnrealTargetConfiguration.Debug && Target.bDebugBuildsActuallyUseDebugCRT;
        if (Target.Platform == UnrealTargetPlatform.Win64)
        {
            LibPath = Path.Combine(PythonPath, "libs");
            isLibrarySupported = true;
        }
        else
        {
            string Err = string.Format("{0} dedicated server is made to depend on {1}. We want to avoid this, please correct module dependencies.", Target.Platform.ToString(), this.ToString());
            System.Console.WriteLine(Err);
        }

        if (isLibrarySupported)
        {
            //Add Include path 
            PublicIncludePaths.AddRange(new string[] { Path.Combine(PythonPath, "include") });

            // Add Library Path 
            PublicLibraryPaths.Add(LibPath);

            //Add Static Libraries
            PublicAdditionalLibraries.Add("python36.lib");

            //Add Dynamic Libraries
            PublicDelayLoadDLLs.Add("python36.dll");
        }

        PublicDefinitions.Add(string.Format("WITH_PYTHON_BINDING={0}", isLibrarySupported ? 1 : 0));

        return isLibrarySupported;
    }
}
