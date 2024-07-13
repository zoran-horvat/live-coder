"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.VsCodeClient = void 0;
const ide_1 = require("../ide-integration/ide");
const dialogs_1 = require("./dialogs");
const vscodememento_1 = require("./vscodememento");
const command_1 = require("./command");
class VsCodeClient extends ide_1.Ide {
    globalState;
    constructor(globalState) {
        super();
        this.globalState = globalState;
    }
    get dialogs() { return new dialogs_1.VsCodeDialogs(); }
    get environment() { return new vscodememento_1.VsCodeMemento(this.globalState); }
    async withNoEditorsOpen() { await command_1.Command.closeAllEditors.execute(); }
    async withWorkspaceOpen(fsPath) { await command_1.Command.openWorkspace(fsPath).execute(); }
    async withExplorerFoldersCollapsed() { await command_1.Command.collapseExplorerFolders.execute(); }
}
exports.VsCodeClient = VsCodeClient;
//# sourceMappingURL=vscodeclient.js.map