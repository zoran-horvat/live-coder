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
exports.command = command;
const vscode = __importStar(require("vscode"));
async function command(ide, fs, environment) {
    console.log('Deploy v. 23:05');
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
    fs.clearDirectoryRecursive(destPath);
    fs.deployDemo(sourcePath, destPath);
    // Open the destination directory in the current VS Code
    vscode.commands.executeCommand('vscode.openFolder', vscode.Uri.file(destPath), false);
}
//# sourceMappingURL=deploy.js.map