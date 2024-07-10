"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.VsCodeClient = void 0;
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
}
exports.VsCodeClient = VsCodeClient;
//# sourceMappingURL=vscodeclient.js.map