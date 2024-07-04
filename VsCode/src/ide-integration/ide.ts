import { Dialogs } from "./dialogs";

export abstract class Ide {
	abstract get dialogs(): Dialogs;
}