import * as React from 'react';
import * as ReactDom from 'react-dom';
import { Version } from '@microsoft/sp-core-library';
import { BaseClientSideWebPart } from '@microsoft/sp-webpart-base';
import {
  IPropertyPaneConfiguration,
  PropertyPaneTextField
} from '@microsoft/sp-property-pane';

import * as strings from 'BrickheadzWebPartStrings';
import Brickheadz, { IBrickheadzProps } from './components/Brickheadz';

export interface IBrickheadzWebPartProps {
  type: string;
}

export default class BrickheadzWebPart extends BaseClientSideWebPart<IBrickheadzWebPartProps> {

  public render(): void {
    const element: React.ReactElement<IBrickheadzProps> = React.createElement(
      Brickheadz,
      {
        type: this.properties.type
      }
    );

    ReactDom.render(element, this.domElement);
  }

  protected onDispose(): void {
    ReactDom.unmountComponentAtNode(this.domElement);
  }

  protected get dataVersion(): Version {
    return Version.parse('1.0');
  }

  protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
    return {
      pages: [
        {
          header: {
            description: strings.PropertyPaneDescription
          },
          groups: [
            {
              groupName: strings.BasicGroupName,
              groupFields: [
                PropertyPaneTextField('type', {
                  label: `Specify the type: boy, girl, all`
                })
              ]
            }
          ]
        }
      ]
    };
  }
}
