export abstract class Dialogs {
	abstract selectDirectoryOrShowError(prompt: string, errorMessage: string): Promise<string | undefined>;
}