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
        if (!fs.existsSync(destinationPath)) {
            fs.mkdirSync(destinationPath, { recursive: true });
        }
        await fs.readdir(sourcePath, { withFileTypes: true }, async (error, entries) => {
            if (!error)
                await this.deployEntries(entries, sourcePath, destinationPath);
        });
    }
    async deployEntries(entries, sourcePath, destinationPath) {
        for (const entry of entries) {
            if (!this.shouldCopy(entry)) {
                continue;
            }
            const finalSourcePath = path.join(sourcePath, entry.name);
            const finalDestinationPath = path.join(destinationPath, entry.name);
            if (entry.isDirectory()) {
                this.deployDemo(finalSourcePath, finalDestinationPath);
            }
            else if (entry.isFile()) {
                fs.copyFileSync(finalSourcePath, finalDestinationPath);
            }
        }
    }
    async clearDirectoryRecursive(directoryPath) {
        if (!fs.existsSync(directoryPath)) {
            return;
        }
        const entries = fs.readdirSync(directoryPath, { withFileTypes: true });
        for (const entry of entries) {
            const entryPath = path.join(directoryPath, entry.name);
            if (entry.isDirectory()) {
                await fs.rmdir(entryPath, { recursive: true }, () => { });
            }
            else if (entry.isFile()) {
                await fs.unlink(entryPath, () => { });
            }
        }
    }
    async ensureDirectoryExists(root, directory) {
        const fullPath = path.join(root, directory);
        if (!fs.existsSync(fullPath)) {
            await fs.mkdir(fullPath, { recursive: true }, () => { });
        }
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