import 'jest';
import Adapter from 'enzyme-adapter-react-16';
import Sinon from 'sinon';
import * as React from 'react';
import { configure, mount, ReactWrapper } from 'enzyme';

import Brickheadz, { IBrickheadzProps } from '../Brickheadz';

configure({ adapter: new Adapter() });

describe('<Brickheadz />', () => {
  let componentWillMountSpy: Sinon.SinonSpy;
  let componentDidMountSpy: Sinon.SinonSpy;
  let componentDidUpdateSpy: Sinon.SinonSpy;
  let renderedElement: ReactWrapper<IBrickheadzProps, {}>;

  beforeEach(() => {
    componentWillMountSpy = Sinon.spy(Brickheadz.prototype, 'componentWillMount');
    componentDidMountSpy = Sinon.spy(Brickheadz.prototype, 'componentDidMount');
    componentDidUpdateSpy = Sinon.spy(Brickheadz.prototype, 'componentDidUpdate');
    renderedElement = mount(<Brickheadz type="boy" />);
  });

  afterEach(() => {
    renderedElement.unmount();
    componentWillMountSpy.restore();
    componentDidMountSpy.restore();
    componentDidUpdateSpy.restore();
  });

  test('<Brickheadz /> should at least render one time', () => {
    expect(componentWillMountSpy.calledOnce).toBeTruthy();
    expect(componentDidMountSpy.calledOnce).toBeTruthy();
  });

  test('<Brickheadz /> should render at least two images', () => {
    expect(renderedElement.find('img')).toBeDefined();
    expect(renderedElement.find('img').length).toBe(2);
  });


  test('<Brickheadz /> test "boy" rendering', () => {
    const stateValue = "boy";
    renderedElement.setState({
      type: stateValue
    });

    // Update the component
    renderedElement.update();

    expect(renderedElement.state('type')).toBe(stateValue);
    expect(renderedElement.find('img').length).toBe(2);
  });

  test('<Brickheadz /> test "girl" rendering', () => {
    const stateValue = "girl";
    renderedElement.setState({
      type: stateValue
    });

    // Update the component
    renderedElement.update();

    expect(renderedElement.state('type')).toBe(stateValue);
    expect(renderedElement.find('img').length).toBe(2);
  });

  test('<Brickheadz /> test "all" rendering', () => {
    const stateValue = "all";
    renderedElement.setState({
      type: stateValue
    });

    // Update the component
    renderedElement.update();

    expect(renderedElement.state('type')).toBe(stateValue);
    expect(renderedElement.find('img').length).toBe(4);
  });

  test('<Brickheadz /> update "girl" property', () => {
    const propValue = "girl";
    renderedElement.setProps({
      type: propValue
    });

    // Update the component
    renderedElement.update();

    expect(componentDidMountSpy.called).toBeTruthy();

    expect(renderedElement.state('type')).toBe(propValue);
    expect(renderedElement.find('img').length).toBe(2);
  });

  test('<Brickheadz /> update with invalid property', () => {
    const propValue = "something";
    renderedElement.setProps({
      type: propValue
    });

    // Update the component
    renderedElement.update();

    expect(renderedElement.state('type')).toBe("boy");
    expect(renderedElement.find('img').length).toBe(2);
  });
});
