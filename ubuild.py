import os
import subprocess

class BuildTarget:
    """A platform that a Unity project can target when building.
    """
    def __init__(self):
        """Creates a new BuildTarget. Note, common targets are already provided via factory methods in this module! For example, target_windows_64().
        """
        self.subfolder = ""
        self.editor_name = ""
        self.build_type = ""
        self.application_extension = ""

# TODO: Test win32, osx, and linux64 builds

def target_windows_64() -> BuildTarget:
    t = BuildTarget()
    t.application_extension = ".exe"
    t.build_type = "-buildWindows64Player"
    t.editor_name = "Unity.exe"
    t.subfolder = "Win64"
    return t

def target_windows_32() -> BuildTarget:
    t = BuildTarget()
    t.application_extension = ".exe"
    t.build_type = "-buildWindowsPlayer"
    t.editor_name = "Unity.exe"
    t.subfolder = "Win32"
    return t

def target_osx() -> BuildTarget:
    t = BuildTarget()
    t.application_extension = ".app"
    t.build_type = "-buildOSXUniversalPlayer"
    t.editor_name = "Unity.app"
    t.subfolder = "OSX"
    return t

def target_linux_64() -> BuildTarget:
    t = BuildTarget()
    t.application_extension = ""
    t.build_type = "-buildLinux64Player"
    t.editor_name = "Unity"
    t.subfolder = "Linux64"
    return t


class UnityBuildSettings:
    """A record of settings that are commonly used when building projects. Cache them here and simplify the project_build function call.
    """
    def __init__(self, editor_folder_path: str, project_path: str) -> None:
        """Creates a new instance of UnityBuildSettings.

        Args:
            editor_folder_path (str): The path to the folder containing all Unity Editor installs. The correct subfolder will automatically be found.
            project_path (str): The path to the root folder of your Unity project that should be built. This is one level higher than the Assets folder.
        """
        self.editor_folder_path = editor_folder_path
        self.project_path = project_path
        self.default_build_name = "Application"
    
def get_unity_version(project_path: str) -> str:
    """Parses a Unity Editor version, given a project on disk.

    Args:
        project_path (str): The path to the root folder of your Unity project. This is one level higher than the Assets folder.

    Returns:
        str: The Unity Editor version for this project.
    """
    version_path = os.path.join(project_path, "ProjectSettings", "ProjectVersion.txt")

    with open(version_path) as version_file:
        # Parse the version file to find the version string. Hopefully this file format doesn't change.
        first_line = version_file.readline().rstrip()
        return first_line.split(" ")[1]

def get_build_output_path(project_path: str, subfolder: str) -> str:
    """Gets a build path that the project_build_settings() function would use. Useful for previewing what will happen during a build.

    Args:
        project_path (str): The path to the root folder of your Unity project. This is one level higher than the Assets folder.
        subfolder (str): The final directory in the path where the build is located, usually changed depending on what platform you are targetting.

    Returns:
        str: The path to a folder where a build generate via project_build_settings() would be placed.
    """
    return os.path.join(project_path, 'Builds', 'UnityLauncherBuild', subfolder)

def project_build_settings(target: BuildTarget, settings: UnityBuildSettings) -> str:
    """Builds a Unity project, using some predefined settings.

    Args:
        target (BuildTarget): The device target that the executable should be built for.
        settings (UnityBuildSettings): The settings to use when building the executable.

    Returns:
        str: The path to the folder where the built executable was stored.
    """
    output_path = get_build_output_path(settings.project_path, target.subfolder)
    project_build(target, output_path, settings.default_build_name, settings.editor_folder_path, settings.project_path)
    return output_path

def project_build(target: BuildTarget, output_path: str, application_name: str, editor_folder_path: str, project_path: str) -> str:
    """Builds a Unity project.

    Args:
        target (BuildTarget): The device target that the executable should be build for.
        output_path (str): The path that the resulting executable should be placed in.
        application_name (str): The name that should be given to the built executable, with no extension on the end.
        editor_folder_path (str): The path to the folder containing all Unity Editor installs. The correct subfolder will automatically be found.
        project_path (str): The path to the Unity project that should be build.
    """
    application_path = os.path.join(output_path, f"{application_name}{target.application_extension}")

    # ensure the output direction exists before building into it
    if not os.path.exists(output_path):
        os.makedirs(output_path)

    version = get_unity_version(project_path)
    unity_path = os.path.join(editor_folder_path, version, "Editor", target.editor_name)
    subprocess.run([unity_path, '-projectPath', project_path, '-batchmode', target.build_type, application_path, '-quit'])