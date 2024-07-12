"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.VsCodeClient = void 0;
const vscode = __importStar(require("vscode"));
const ide_1 = require("../ide-integration/ide");
const dialogs_1 = require("./dialogs");
const vscodememento_1 = require("./vscodememento");
class VsCodeClient extends ide_1.Ide {
    globalState;
    constructor(globalState) {
        super();
        this.globalState = globalState;
    }
    get dialogs() { return new dialogs_1.VsCodeDialogs(); }
    get environment() { return new vscodememento_1.VsCodeMemento(this.globalState); }
    async withNoEditorsOpen() {
        this.getAllTabs().map(this.toUri).forEach(await this.closeDocument);
    }
    getAllTabs() {
        return vscode.window.tabGroups.all.map(group => group.tabs).flat();
    }
    toUri(tab) {
        if (tab.input instanceof vscode.TabInputText) {
            return tab.input.uri;
        }
        return undefined;
    }
    async closeDocument(uri) {
        if (!uri) {
            return;
        }
        await vscode.window.showTextDocument(uri, { preview: false, preserveFocus: false });
        await vscode.commands.executeCommand("workbench.action.closeActiveEditor");
    }
}
exports.VsCodeClient = VsCodeClient;
//# sourceMappingURL=vscodeclient.js.map