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
exports.LocalFileSystem = void 0;
const fs = __importStar(require("fs"));
const path = __importStar(require("path"));
const filesystem_1 = require("./filesystem");
class LocalFileSystem extends filesystem_1.FileSystem {
    deployDemo(sourcePath, destinationPath) {
        if (!fs.existsSync(destinationPath)) {
            fs.mkdirSync(destinationPath, { recursive: true });
        }
        const entries = fs.readdirSync(sourcePath, { withFileTypes: true });
        for (const entry of entries) {
            const finalSourcePath = path.join(sourcePath, entry.name);
            const finalDestinationPath = path.join(destinationPath, entry.name);
            if (entry.isDirectory()) {
                this.deployDemo(finalSourcePath, finalDestinationPath);
            }
            else if (entry.isFile()) {
                fs.copyFileSync(sourcePath, finalDestinationPath);
            }
        }
    }
    clearDirectoryRecursive(directoryPath) {
        if (!fs.existsSync(directoryPath)) {
            return;
        }
        const entries = fs.readdirSync(directoryPath, { withFileTypes: true });
        for (const entry of entries) {
            const entryPath = path.join(directoryPath, entry.name);
            if (entry.isDirectory()) {
                this.clearDirectoryRecursive(entryPath);
                fs.rmdirSync(entryPath);
            }
            else if (entry.isFile()) {
                fs.unlinkSync(entryPath);
            }
        }
    }
}
exports.LocalFileSystem = LocalFileSystem;
//# sourceMappingURL=localfilesystem.js.map