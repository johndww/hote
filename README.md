# hote
Heros of the Empire - Unity3d Mobile Game

Git LFS is required for this project. Download it at https://git-lfs.github.com/ .  Make sure it's downloaded and installed prior to cloning this repo.

Blender is required for this project.  Download it at https://www.blender.org/ .

It doesn't appear to be possible to merge unity project conflicts because they are stored as blobs.  We have metadata files enabled which allows git to track changes to the project, but the merges are unreadable.  This means that we can't really work on project files concurrently.  If you have a merge conflict, create a new branch and manually re-apply your changes on top of the latest.

------

Fixing common issues:

One common issue is that the level's look empty (no textures) or that the characters are missing their animations (characters are probably invisible).  This likely means that you opened Unity prior to installing Blender.  To fix this, close Unity, make sure Blender is installed, delete the Library folder in your repo (part of .gitignore anyways) and re-open Unity.  Unity will regenerate the Library directory, and properly import the blender files.
