import React, { useEffect } from 'react';
import PropTypes from 'prop-types';


export const MobileWinLossCellContent = (props) => {
    const { item } = props;

    useEffect(() => {}, [item]);

    return (
        <div>
            <div dangerouslySetInnerHTML={{ __html: item.WinLoss }}></div>
            <div dangerouslySetInnerHTML={{ __html: item.Status }}></div>
        </div>
    );
};

const propTypes = {
    item: PropTypes.object
};

MobileWinLossCellContent.propTypes = propTypes;

export default MobileWinLossCellContent