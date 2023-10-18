import { isDesktop } from 'react-device-detect';

export const useMobileVersion = () => {
    const width = window.innerWidth;
    if (isDesktop)
    {
        return false;
    }
    return width < 1024;
}

export default useMobileVersion;