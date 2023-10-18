import React, { useEffect } from 'react';
import PropTypes from 'prop-types';


export const MobileBetlistContent = (props) => {
    const { item } = props;

    useEffect(() => {}, [item]);
    return (
        <div>
            <div><span className='cell-index'>#{item.Index}</span> - <span dangerouslySetInnerHTML={{__html: item.TransTime}}></span></div>
            <div>{item.RefNo}</div>
            <div dangerouslySetInnerHTML={{ __html: item.Choice }}></div>
        </div>
    );
};

const propTypes = {
    item: PropTypes.object
};

MobileBetlistContent.propTypes = propTypes;

export default MobileBetlistContent