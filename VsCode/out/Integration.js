"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Integration = void 0;
const localfilesystem_1 = require("./fs-integration/localfilesystem");
const vscodeclient_1 = require("./vscode-integration/vscodeclient");
const vscodememento_1 = require("./vscode-integration/vscodememento");
class Integration {
    static ideInstance;
    static fsInstanced;
    static environmentInstance;
    static globalState;
    constructor(globalState) {
        Integration.globalState = globalState;
    }
    get ide() {
        if (!Integration.ideInstance) {
            Integration.ideInstance = new vscodeclient_1.VsCodeClient(Integration.globalState);
        }
        return Integration.ideInstance;
    }
    get fs() {
        if (!Integration.fsInstanced) {
            Integration.fsInstanced = new localfilesystem_1.LocalFileSystem();
        }
        return Integration.fsInstanced;
    }
    get environment() {
        if (!Integration.environmentInstance) {
            Integration.environmentInstance = new vscodememento_1.VsCodeMemento(Integration.globalState);
        }
        return Integration.environmentInstance;
    }
}
exports.Integration = Integration;
//# sourceMappingURL=integration.js.map