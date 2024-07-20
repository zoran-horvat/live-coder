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
    async deployDemo(sourcePath, destinationPath) {
        try {
            if (!fs.existsSync(destinationPath)) {
                fs.mkdirSync(destinationPath);
            }
            for (var entry of fs.readdirSync(sourcePath, { withFileTypes: true })) {
                if (!this.shouldCopy(entry)) {
                    continue;
                }
                const entrySourcePath = path.join(sourcePath, entry.name);
                const entryDestinationPath = path.join(destinationPath, entry.name);
                if (entry.isDirectory()) {
                    this.deployDemo(entrySourcePath, entryDestinationPath);
                }
                else if (entry.isFile()) {
                    fs.copyFileSync(entrySourcePath, entryDestinationPath);
                }
            }
        }
        catch (err) {
            console.log('Error occurred: ' + err);
        }
    }
    async clearDirectoryRecursive(directoryPath) {
        if (!fs.existsSync(directoryPath)) {
            return;
        }
        try {
            for (const entry of fs.readdirSync(directoryPath, { withFileTypes: true })) {
                const entryPath = path.join(directoryPath, entry.name);
                if (entry.isDirectory()) {
                    await fs.rmdirSync(entryPath, { recursive: true });
                }
                else if (entry.isFile()) {
                    await fs.unlinkSync(entryPath);
                }
            }
        }
        catch (err) { }
    }
    asString(err) {
        if (err instanceof Error) {
            return err.message;
        }
        return JSON.stringify(err);
    }
    async ensureDirectoryExists(root, directory) {
        const fullPath = directory ? path.join(root, directory) : root;
        if (fs.existsSync(fullPath)) {
            return fullPath;
        }
        await fs.mkdir(fullPath, { recursive: true }, () => { });
        return fullPath;
    }
    async ensureTextFileExists(root, fileName, defaultContent) {
        const fullPath = fileName ? path.join(root, fileName) : root;
        if (fs.existsSync(fullPath)) {
            return fullPath;
        }
        await fs.writeFile(fullPath, defaultContent || '', () => { });
        return fullPath;
    }
    async getExistingFilePath(root, fileName) {
        const fullPath = path.join(root, fileName);
        if (fs.existsSync(fullPath)) {
            return Promise.resolve(fullPath);
        }
        return Promise.resolve(null);
    }
    shouldCopy(entry) {
        if (entry.name.startsWith('.')) {
            return false;
        }
        if (entry.isDirectory() && this.ignoreDirectories.includes(entry.name)) {
            return false;
        }
        return true;
    }
    get ignoreDirectories() {
        return ['bin', 'obj'];
    }
}
exports.LocalFileSystem = LocalFileSystem;
//# sourceMappingURL=localfilesystem.js.map