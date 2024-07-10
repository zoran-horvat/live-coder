export abstract class Environment {
    abstract get lastSourcePath(): string | undefined;
    abstract set lastSourcePath(path: string | undefined);
    
    abstract get lastDestPath(): string | undefined;
    abstract set lastDestPath(path: string | undefined);
}