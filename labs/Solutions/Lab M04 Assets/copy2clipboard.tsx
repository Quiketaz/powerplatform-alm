import * as React from 'react';
import * as CopyToClipboard from 'react-copy-to-clipboard';
import { ColorClassNames } from '@uifabric/styling';
import {getTheme, MessageBar, MessageBarType,IconButton,TextField} from 'office-ui-fabric-react';
 
export interface ITextFieldProps{
    onInputChanged?:(newValue:string)=>void;
    inputValue?: string;
    successVisible?: boolean;
}
 
export interface ITextFieldControlledExampleState extends React.ComponentState,ITextFieldProps{}
const theme = getTheme();
export class TextFieldControlledExample extends React.Component<ITextFieldProps, ITextFieldControlledExampleState> {
    
    constructor(props:ITextFieldProps){
        super(props);
 
        this.state = {
            inputValue: props.inputValue?props.inputValue:'',
            successVisible: false
        };
    }
 
    render():JSX.Element{
        const {successVisible} = this.state;
    return (
        <div className='wrapper'>
        
        <div className='inner'>
        <TextField  
            value={this.state.inputValue}
            placeholder = '---'
            onChange = {this.onChangeText}
            styles = {{ field:{ fontWeight: "bold", borderColor:ColorClassNames.whiteTranslucent40 },
            fieldGroup:  {borderColor:theme.palette.whiteTranslucent40,borderTopColor: theme.palette.whiteTranslucent40},
            wrapper: {borderColor:ColorClassNames.whiteTranslucent40},
            }}
            className='reactinput' 
            />
        <CopyToClipboard text={this.state.inputValue||''}
          onCopy={() => this.setState({copied: true,
          successVisible: true})}>
          <IconButton
                className='reactbutton'
                title='Click to Copy'
                iconProps={{ iconName: 'Copy' }}
                onClick={this._onIconClick}
                styles={{ root: { color:ColorClassNames.neutralDark },
                rootHovered: {color:ColorClassNames.blackHover},
                rootPressed: {color:ColorClassNames.blackTranslucent40}   }}
      />
        </CopyToClipboard>
        </div>
        {successVisible? (<MessageBar
            onDismiss={this.onDismissSuccess}
            dismissButtonAriaLabel="Dismiss"
            messageBarType={MessageBarType.success}
        >{this.props.inputValue} copied to clipboard</MessageBar>
        ) : null}
        </div>
        );
    }
 
    private onChangeText = (event: React.FormEvent<HTMLInputElement|HTMLTextAreaElement>, newValue?: string) => {
        this.setState({ inputValue: newValue || '' });
        if(this.props.onInputChanged){
            this.props.onInputChanged(newValue||'')
        }
    };
    private _onIconClick = ():void=>{
    
    };
    private onDismissSuccess = ():void=>{
        this.setState({successVisible:false})
    }
    private showSuccessMessage = ():void=>{
        this.setState({successVisible:true});
    }
}