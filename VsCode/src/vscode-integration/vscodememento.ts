import * as vscode from 'vscode';
import { Environment } from "../ide-integration/environment";

export class VsCodeMemento extends Environment {
    private globalState: vscode.Memento;

    private sourcePathKey: string = 'path.source';
    private destPathKey: string = 'path.dest';

    constructor(globalState: vscode.Memento) {
        super();
        this.globalState = globalState;
    }
    
    get lastSourcePath(): string | undefined {
        return this.globalState.get(this.sourcePathKey);
    }

    set lastSourcePath(path: string | undefined) {
        this.globalState.update(this.sourcePathKey, path);
    }

    get lastDestPath(): string | undefined {
        return this.globalState.get(this.destPathKey);
    }
    set lastDestPath(path: string | undefined) {
        this.globalState.update(this.destPathKey, path);
    }
}