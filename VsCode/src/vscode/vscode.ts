import { Dialogs } from "../ide/dialogs";
import { Ide } from "../ide/ide";
import { VsCodeDialogs } from "./dialogs";

export class VsCode extends Ide {
	get dialogs() : Dialogs { return new VsCodeDialogs(); }
}