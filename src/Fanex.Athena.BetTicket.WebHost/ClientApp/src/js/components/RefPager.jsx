import React, { forwardRef }  from "react";
import { Pager } from '@9softstudio/react-grid';

export const RefPager = forwardRef(function RefPager(props, ref){
    return (
    <div ref={ref} className="page-wrapper-container">
        <Pager {...props}/>
    </div>)
})