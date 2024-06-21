import {IInputs, IOutputs} from "./generated/ManifestTypes";
import { ITextFieldProps,TextFieldControlledExample } from "./copy2clipboard";
import * as ReactDOM from "react-dom";
import * as React from "react";
import {initializeIcons} from '@uifabric/icons';
import { isNullOrUndefined } from "util";
 
export class CopyText implements ComponentFramework.StandardControl<IInputs, IOutputs> {
 
    private notifyOutputChanged: () => void;
    private _container: HTMLDivElement;
    private _input: string;
    private _inputElement: React.ReactElement;
    private props: ITextFieldProps = {
        onInputChanged: this.inputValueChanged.bind(this)
    };
    private inputValueChanged(newValue:string){
            this.props.inputValue = newValue;
            this._input = newValue;
            this.notifyOutputChanged();
    }
 
    /**
     * Used to initialize the control instance. Controls can kick off remote server calls and other initialization actions here.
     * Data-set values are not initialized here, use updateView.
     * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to property names defined in the manifest, as well as utility functions.
     * @param notifyOutputChanged A callback method to alert the framework that the control has new outputs ready to be retrieved asynchronously.
     * @param state A piece of data that persists in one session for a single user. Can be set at any point in a controls life cycle by calling 'setControlState' in the Mode interface.
     * @param container If a control is marked control-type='standard', it will receive an empty div element within which it can render its content.
     */
    public init(context: ComponentFramework.Context<IInputs>, notifyOutputChanged: () => void, state: ComponentFramework.Dictionary, container:HTMLDivElement)
    {
        initializeIcons();
        this._container = container;
        this.notifyOutputChanged = notifyOutputChanged;
        if(!(context.parameters._inputProperty === null || undefined) && ! (context.parameters._inputProperty.raw === null || undefined)){
            this._input = context.parameters._inputProperty.raw || "";
            this.props.inputValue = this._input;
            
        }
    }
 
    /**
     * Called when any value in the property bag has changed. This includes field values, data-sets, global values such as container height and width, offline status, control metadata values such as label, visible, etc.
     * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to names defined in the manifest, as well as utility functions
     */
    public updateView(context: ComponentFramework.Context<IInputs>): void
    {
        if(this._input != context.parameters._inputProperty.raw || ((this._input === null || undefined) && !(context.parameters._inputProperty.raw === null || undefined))){
            this._input = context.parameters._inputProperty.raw||"";
        }
        ReactDOM.render(
            this._inputElement = React.createElement(TextFieldControlledExample,this.props),
            this._container
        );
    }
 
    /** 
     * It is called by the framework prior to a control receiving new data. 
     * @returns an object based on nomenclature defined in manifest, expecting object[s] for property marked as “bound” or “output”
     */
    public getOutputs(): IOutputs
    {
        return {
            _inputProperty:this._input
        };
    }
 
    /** 
     * Called when the control is to be removed from the DOM tree. Controls should use this call for cleanup.
     * i.e. cancelling any pending remote calls, removing listeners, etc.
     */
    public destroy(): void
    {
        // Add code to cleanup control if necessary
    }
}