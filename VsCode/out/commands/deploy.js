"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.command = command;
const script_1 = require("../scripting/script");
const noeditorsopen_1 = require("../scripting/specifications/noeditorsopen");
const workspaceopen_1 = require("../scripting/specifications/workspaceopen");
const explorerFoldersCollapsed_1 = require("../scripting/specifications/explorerFoldersCollapsed");
async function command(ide, fs, environment) {
    // Select the source directory
    const sourcePath = await ide.dialogs.selectDirectoryOrShowError('Select Source Directory', "No source directory selected.", environment.lastSourcePath);
    if (!sourcePath) {
        return;
    }
    // Select the destination directory
    const destPath = await ide.dialogs.selectDirectoryOrShowError('Select Destination Directory', "No destination directory selected.", environment.lastDestPath);
    if (!destPath) {
        return;
    }
    environment.lastSourcePath = sourcePath;
    environment.lastDestPath = destPath;
    // Copy the source directory to the destination directory
    await fs.clearDirectoryRecursive(destPath);
    await fs.deployDemo(sourcePath, destPath);
    await executePrelude(ide, fs, destPath);
}
async function executePrelude(ide, fs, workspacePath) {
    let prelude = new script_1.Script();
    prelude.append(new noeditorsopen_1.NoEditorsOpen());
    prelude.append(new explorerFoldersCollapsed_1.ExplorerFoldersCollapsed());
    prelude.append(new workspaceopen_1.WorkspaceOpen(workspacePath));
    await prelude.execute(ide, fs);
}
//# sourceMappingURL=deploy.js.map