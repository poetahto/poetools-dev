import os
import time
import subprocess
from ubuild import *


# TODO: this needs to be changed for each user of this script, not very good or convienient
# maybe search or prompt, and save results to a gitignored file?
editor_folder_path = r"C:\Program Files\Unity\Hub\Editor"

# ensure that this script is always in the unity project root, do we need it to be more flexible?
project_path = os.path.dirname(__file__)
output_path = get_build_output_path(project_path, "[platform]")
version = get_unity_version(project_path)

# TODO: the build name will also probably want to change on a per-project basis, maybe add in setup stuff?
settings = UnityBuildSettings(editor_folder_path, project_path)

print(f"\nProject: {project_path}\nOutput: {output_path}\nVersion: {version}\n")
print(f"Do you want to make a build right now?")
answer = input()

if answer == "y" or answer == "yes":
    # TODO: store build times and generate estimate?
    print("\nStarted building the project... (this may take a while)")
    startTime = time.perf_counter()
    result = project_build_settings(target_windows_64(), settings)
    endTime = time.perf_counter()
    duration = endTime - startTime
    print(f"Finished building! Build took {duration:0.4f} seconds.")
    subprocess.call(["explorer", result])

else:
    print("\nNo build was created.")
