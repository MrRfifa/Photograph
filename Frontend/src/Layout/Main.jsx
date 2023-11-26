import { TypeAnimation } from "react-type-animation";
import { LabelDestinationLinkButton } from "../Components/Buttons/CustomizedButtons";

const Main = () => {
  return (
    <div className="text-[#9D4EDD] ">
      <div className="w-full max-w-[1240px] mx-auto mt-[-96px] text-center h-screen flex flex-col justify-center">
        <p className="text-[#9F86C0] font-bold p-2">
          GROWING WITH LOVING IMAGES{" "}
        </p>
        <h1 className="md:text-7xl sm:text-6xl text-4xl font-bold md:py-6">
          Grow with camera.
        </h1>
        <div className="flex flex-row justify-center items-center">
          <p className="md:text-5xl sm:text-4xl text-xl font-bold py-4">
            Stunning, creative designs for
          </p>
          <TypeAnimation
            sequence={[
              "nature",
              1000,
              "humans",
              1000,
              "animals",
              1000,
              "...",
              1000,
            ]}
            wrapper="span"
            className="md:text-5xl sm:text-4xl text-xl font-bold pl-2 md:pl-4 text-[#E0AAFF]"
            speed={10}
            repeat={Infinity}
            deletionSpeed={20}
          />
        </div>
        <p className="md:text-2xl text-xl font-bold text-[#E0AAFF]">
          Optimize your photography techniques to capture stunning images and
          moments with your camera.
        </p>
        <LabelDestinationLinkButton
          nav={false}
          label="Get Started"
          destination="/register"
        />
      </div>
    </div>
  );
};

export default Main;
