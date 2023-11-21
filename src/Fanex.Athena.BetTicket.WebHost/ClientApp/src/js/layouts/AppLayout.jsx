import React from 'react';
import classNames from 'classnames';

export const AppLayout = (props) => {
    const className = classNames('container-fluid', props.className)
    return (
      <div id="container" className={className}>
        {props.children}
      </div>
    );
}

export default AppLayout;