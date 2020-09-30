import * as React from 'react';
import styles from './Brickheadz.module.scss';
import { TypeCheck } from './helper/TypeCheck';

export type BoyGirlType = "boy" | "girl" | "all";

export interface IBrickheadzProps {
  type: string;
}

export interface IBrickheadzState {
  type: BoyGirlType;
}

export default class Brickheadz extends React.Component<IBrickheadzProps, IBrickheadzState> {

  constructor(props: IBrickheadzProps) {
    super(props);

    this.state = {
      type: "boy"
    };
  }

  public componentWillMount(): void {
    const { type } = this.props;
    if (type && this.checkType(type)) {
      this.setState({
        type: type as BoyGirlType
      });
    }
  }

  public componentDidMount(): void {
    // Not used atm
  }

  public componentDidUpdate(prevProps: IBrickheadzProps, prevState: IBrickheadzState): void {
    const { type } = this.props;
    if (prevProps.type !== type && TypeCheck.checkType(type)) {
      this.setState({
        type: type as BoyGirlType
      });
    }
  }


  public render(): React.ReactElement<IBrickheadzProps> {
    return (
      <div className={ styles.brickheadz } data-ui-test-id="brickheadz">
        <div className={ styles.container }>
          <div className={ styles.row } data-ui-test-id="brickheadz-row">
            {
              this.state.type === "all" ? (
                <>
                  <div className={ styles.column }>
                    <img src={require(`./images/boy1.png`)} />
                  </div>
                  <div className={ styles.column }>
                    <img src={require(`./images/girl1.png`)} />
                  </div>
                  <div className={ styles.column }>
                    <img src={require(`./images/girl2.png`)} />
                  </div>
                  <div className={ styles.column }>
                    <img src={require(`./images/boy2.png`)} />
                  </div>
                </>
              ) : (
                <>
                  <div className={ styles.column }>
                    <img src={require(`./images/${this.state.type}1.png`)} />
                  </div>
                  <div className={ styles.column }>
                    <img src={require(`./images/${this.state.type}2.png`)} />
                  </div>
                </>
              )
            }
          </div>
        </div>
      </div>
    );
  }

  private checkType(type: string): boolean {
    switch (type) {
      case "boy":
      case "girl":
      case "all":
        return true;
      default:
        return false;
    }
  }
}
