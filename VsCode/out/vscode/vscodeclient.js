"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.VsCode = void 0;
const ide_1 = require("../ide/ide");
const dialogs_1 = require("./dialogs");
class VsCode extends ide_1.Ide {
    get dialogs() { return new dialogs_1.VsCodeDialogs(); }
}
exports.VsCode = VsCode;
//# sourceMappingURL=vscodeclient.js.map