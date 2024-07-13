"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.VsCodeMemento = void 0;
const environment_1 = require("../ide-integration/environment");
class VsCodeMemento extends environment_1.Environment {
    globalState;
    sourcePathKey = 'path.source';
    destPathKey = 'path.dest';
    recordingPathKey = 'path.recording';
    constructor(globalState) {
        super();
        this.globalState = globalState;
    }
    get lastSourcePath() {
        return this.globalState.get(this.sourcePathKey);
    }
    set lastSourcePath(path) {
        this.globalState.update(this.sourcePathKey, path);
    }
    get lastDestPath() {
        return this.globalState.get(this.destPathKey);
    }
    set lastDestPath(path) {
        this.globalState.update(this.destPathKey, path);
    }
    get lastRecordingPath() {
        return this.globalState.get(this.recordingPathKey);
    }
    set lastRecordingPath(path) {
        this.globalState.update(this.recordingPathKey, path);
    }
}
exports.VsCodeMemento = VsCodeMemento;
//# sourceMappingURL=vscodememento.js.map