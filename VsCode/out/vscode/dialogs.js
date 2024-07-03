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
exports.VsCodeDialogs = void 0;
const vscode = __importStar(require("vscode"));
const dialogs_1 = require("../ide/dialogs");
class VsCodeDialogs extends dialogs_1.Dialogs {
    async selectDirectoryOrShowError(prompt, errorMessage) {
        const uri = await this.selectDirectory(prompt);
        if (!uri) {
            vscode.window.showErrorMessage(errorMessage);
        }
        return uri ? uri.fsPath : undefined;
    }
    async selectDirectory(prompt) {
        const options = {
            canSelectFiles: false,
            canSelectFolders: true,
            canSelectMany: false,
            openLabel: prompt
        };
        const uris = await vscode.window.showOpenDialog(options);
        return uris && uris.length > 0 ? uris[0] : undefined;
    }
}
exports.VsCodeDialogs = VsCodeDialogs;
// export async function selectDirectoryOrShowError(prompt: string, errorMessage: string) {
//     const uri = await selectDirectory(prompt);
//     if (!uri) { vscode.window.showErrorMessage(errorMessage); }
//     return uri;
// }
//# sourceMappingURL=dialogs.js.map