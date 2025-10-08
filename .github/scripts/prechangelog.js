// Force the version/tag used by the action to GitVersion’s output
module.exports.preVersionGeneration = (version) => {
  const gv = process.env.GITVERSION_SEMVER || version;   // 1.2.3
  return gv;
};
module.exports.preTagGeneration = (tag) => {
  const prefix = process.env.TAG_PREFIX || 'v';
  const gv = process.env.GITVERSION_SEMVER || tag.replace(/^v?/, '');
  return `${prefix}${gv}`;                               // v1.2.3
};
